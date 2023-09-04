namespace CalcHTN;




class Program
{
    
    static void Main(string[] args)
    {
        //string partNumber,smiles ;
        //partNumber = args[0].Split(":")[0];
        //smiles = args[0].Split(":")[1];

        new HTSGenerator("3000-3","CCCCC").CalculateTarrifCode();
        
    }
}
