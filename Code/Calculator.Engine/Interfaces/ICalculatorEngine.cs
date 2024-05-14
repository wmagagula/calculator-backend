using Calculator.Engine.Dto;

namespace Calculator.Engine.Interfaces;

public interface ICalculatorEngine
{
    ExpressionResultDto Calculate(ExpressionDto expression);
}
