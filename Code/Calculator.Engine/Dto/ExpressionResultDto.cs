namespace Calculator.Engine.Dto;

public class ExpressionResultDto
{
    public double ExpressionResult { get; }

    public ExpressionDto ExpressionDto { get; }

    public DateTime Date { get; }

    public ExpressionResultDto(double expressionResult, ExpressionDto expressionDto)
    {
        ExpressionDto = expressionDto;
        ExpressionResult = expressionResult;
        Date = DateTime.Now;
    }

}
