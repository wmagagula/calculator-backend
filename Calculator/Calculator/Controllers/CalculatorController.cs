using Calculator.Engine.Dto;
using Calculator.Engine.Enums;
using Calculator.Engine.Exceptions;
using Calculator.Engine.Interfaces;
using Calculator.WebApi.Models;
using Calculator.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CalculatorController : ControllerBase
{
    private readonly ICalculatorEngine _calculator;
    private readonly ICalculatorStateRepository _calculatorStateRepository;

    public CalculatorController(ICalculatorStateRepository calculatorStateRepository, ICalculatorEngine calculator)
    {
        _calculator = calculator;
        _calculatorStateRepository = calculatorStateRepository;
    }

    [HttpGet("history")]
    public List<ComputeResponse> Get()
    {
        return _calculatorStateRepository.GetComputeHistory()
            .Select(e => new ComputeResponse(
                IsSuccess: true,
                Message: "Successful compute",
                ComputeResult: e.ExpressionResult,
                ExpressionDto: e.ExpressionDto,
                Date: e.Date))
            .ToList();
    }
    
    [HttpDelete("history")]
    public bool Delete()
    {
        return _calculatorStateRepository.ClearComputeHistory();
    }
    
    [HttpDelete("history/recall")]
    public bool ClearRecall()
    {
        return _calculatorStateRepository.ClearRecallExpression();
    }

    [HttpPost("compute")]
    public IActionResult Calculate([FromBody] ExpressionDto expressionDto) {

        if (expressionDto is not ExpressionDto { FirstOperand: double, SecondOperand: _,  Operator: Operators }) {
            return BadRequest(error: new ComputeResponse(IsSuccess: false,
                Message: "Specify first and second operand, along with the operator.",
                null,
                null,
                null));
        }

        ExpressionDto? recallPossible = null;

        if(expressionDto.SecondOperand is null && _calculatorStateRepository.CanRecallExpression()) {
            double expressionResult = _calculatorStateRepository.GetRecallExpression().ExpressionResult;
            recallPossible = new ExpressionDto(expressionDto.FirstOperand, expressionResult, expressionDto.Operator);
        }

        try
        {

            var result = _calculator.Calculate(recallPossible ?? expressionDto);
            if (result is not null)
            {
                _calculatorStateRepository.UpdateState(result);

                return Ok(new ComputeResponse(
                    IsSuccess: true,
                    Message: $"Performed operation: {result.ExpressionDto.Operator} on first: {expressionDto.FirstOperand} and second: {recallPossible?.SecondOperand ?? expressionDto.SecondOperand} operand.",
                    ComputeResult: result.ExpressionResult,
                    ExpressionDto: result.ExpressionDto,
                    Date: result.Date));
            }

            return BadRequest(error: new ComputeResponse(
                    IsSuccess: false,
                    Message: $"Could not perform operation: {expressionDto.Operator} on first: {expressionDto.FirstOperand} and second: {expressionDto.SecondOperand} operand.",
                    null, null, null));
        }
        catch (ComputeException ex)
        {
            return BadRequest(error: new ComputeResponse(
                   IsSuccess: false,
                   Message: ex.Message,
                   null, null, null));
        }
        catch (Exception ex)
        {
            return BadRequest(error: new ComputeResponse(IsSuccess: false, Message: ex.Message, null, null, null));
        }
    }
}
