using Calculator.Engine.Dto;

namespace Calculator.WebApi.Repository;

public interface ICalculatorStateRepository
{
    void UpdateState(ExpressionResultDto expressionResult);

    List<ExpressionResultDto> GetComputeHistory();

    bool CanRecallExpression();
    
    ExpressionResultDto? GetRecallExpression();

    bool ClearComputeHistory();

    bool ClearRecallExpression();
}
