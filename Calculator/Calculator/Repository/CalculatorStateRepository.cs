using Calculator.Engine.Dto;

namespace Calculator.WebApi.Repository;

public class CalculatorStateRepository : ICalculatorStateRepository
{
    private ExpressionResultDto? _recallExpression;

    private List<ExpressionResultDto> _computeHistory = [];

    public ExpressionResultDto? RecallExpression { get => _recallExpression; set => _recallExpression = value; }

    public List<ExpressionResultDto> ComputeHistory { get => _computeHistory; set => _computeHistory = value; }

    public void UpdateState(ExpressionResultDto expressionResult)
    {
        RecallExpression = expressionResult;
        ComputeHistory.Add(expressionResult);
    }

    public List<ExpressionResultDto> GetComputeHistory() { return ComputeHistory; }

    public ExpressionResultDto? GetRecallExpression() { return RecallExpression; }
    public bool CanRecallExpression() { return RecallExpression is not null; }

    public bool ClearComputeHistory()
    {
        ComputeHistory.Clear();
        ClearRecallExpression();
        return true;
    }

    public bool ClearRecallExpression()
    {
        _recallExpression = null;
        return true;
    }
}
