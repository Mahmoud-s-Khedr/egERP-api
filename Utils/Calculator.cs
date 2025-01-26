namespace EG_ERP.Utils;

public interface ICalculator
{
    int CalculateTax(decimal totalSalary);
}

public class Calculator : ICalculator
{
    public int CalculateTax(decimal totalSalary)
    {
        return 0;
    }
}
