using MoleculeClassifications;

public class PartNumberHandler
{
    
    private string _pn;
    private int _polyvalentcode;
    private int _structurecode; // Heterocycles = 10 instead of H
    private int fgcode;
    private char monovalentCode;
    private List<HTSCategory> _categories = new List<HTSCategory>(); // if only one item this contains exact match

    PolyvalentCode Polyvalency;
    StructureCode  StructureCode;

    public PartNumberHandler(string partNumber)
    {
        _pn = partNumber;
        monovalentCode = partNumber[5];
        if(!int.TryParse(partNumber[0].ToString(),out _polyvalentcode))
        {
            Console.WriteLine("[ERROR] Invalid PN, please check 1st charachter in PN");
            Environment.Exit(2);
        }
        if(!int.TryParse(partNumber[1].ToString(),out _structurecode))
        {
            if (partNumber[1] == 'H') _structurecode = 10;
            else
            {
                Console.WriteLine("[ERROR] Invalid PN, please check 2nd character in PN");
                Environment.Exit(2);
            }
        }
        
        if(!int.TryParse(partNumber[2..4],out fgcode))
        {
            Console.WriteLine("[ERROR] Invalid PN, please check 3rd & 4th characters in PN");
            Environment.Exit(2);
        }
    
        Polyvalency = (PolyvalentCode) _polyvalentcode;
        StructureCode = (StructureCode) _structurecode;

        if (StructureCode == StructureCode.NONCARBONACEOUS)
        {
            Console.WriteLine("[ERROR] Program does not support PNs with Structure code \"{0}\"",StructureCode);
            Environment.Exit(2);
        }
    }

    public void EvaluateForCategory()
    {
        
        if (Polyvalency == PolyvalentCode.CARBON_ONLY) _categories.Add(HTSCategory.I_HYDROCARBONS);
        else if (Polyvalency == PolyvalentCode.CARBON_OXYGEN)
        {
            // Posssible Categories:
            // II_ALCHOHOLS, III_PHENOLS, IV_ETHERS, V_ALDEHYDES, VI_KETONES, VII_CARBOX_ACIDS, X_ORGANOINORGANICS_HETEROCYCLES
            // Others not supported for detection (ever?):
            // XI_VITAMINS_HORMONES, XII_GLYCOSIDES_ALKALOIDS
            if (StructureCode == StructureCode.ALKANES ||
                StructureCode == StructureCode.CYCLOALKANES ||
                StructureCode == StructureCode.ALKENES ||
                StructureCode == StructureCode.CYCLOALKENES ||
                StructureCode == StructureCode.ALKYNES )
            {
                if (fgcode < 6) _categories.Add(HTSCategory.II_ALCHOHOLS);
                else if (fgcode >= 6 && fgcode < 12) _categories.Add(HTSCategory.IV_ETHERS);
                else if (fgcode == 12) _categories.Add(HTSCategory.XIII_OTHER_ORGANICS); //This is a sugar
                // skipped 13 Hypohalites, (halohydrin), ROX
                else if (fgcode == 14) 
                {
                    _categories.Add(HTSCategory.VII_CARBOX_ACIDS);
                    _categories.Add(HTSCategory.IV_ETHERS);
                }
                else if (fgcode == 15 || fgcode == 16) _categories.Add(HTSCategory.V_ALDEHYDES);
                else if (fgcode == 17 || fgcode == 18 || fgcode == 19 || fgcode == 20 ) _categories.Add(HTSCategory.VI_KETONES);
                else if (fgcode >= 21 && fgcode <= 29) _categories.Add(HTSCategory.VII_CARBOX_ACIDS);
            }


            else if (StructureCode == StructureCode.AROMATICS ||
                StructureCode == StructureCode.ARYL_ALKANES ||
                StructureCode == StructureCode.ARYL_TRIFLUOROMETHYL ||
                StructureCode == StructureCode.ARYL_ALKENES_ALKYNES)
                {
                    // Only the alchohols are considered for phenols (fgcode < 6) other FG's have higher prio categories
                    if (fgcode < 6)
                    {
                        _categories.Add(HTSCategory.III_PHENOLS);
                        _categories.Add(HTSCategory.II_ALCHOHOLS);
                    }
                    else if (fgcode >= 6 && fgcode < 12) _categories.Add(HTSCategory.IV_ETHERS);
                    else if (fgcode == 12) _categories.Add(HTSCategory.XIII_OTHER_ORGANICS); //This is a sugar
                    // skipped 13 Hypohalites, (halohydrin), ROX
                    else if (fgcode == 14) 
                {
                    _categories.Add(HTSCategory.VII_CARBOX_ACIDS);
                    _categories.Add(HTSCategory.IV_ETHERS);
                }
                    else if (fgcode == 15 || fgcode == 16) _categories.Add(HTSCategory.V_ALDEHYDES);
                    else if (fgcode == 17 || fgcode == 18 || fgcode == 19 || fgcode == 20 ) _categories.Add(HTSCategory.VI_KETONES);
                    else if (fgcode >= 21 && fgcode <= 29) _categories.Add(HTSCategory.VII_CARBOX_ACIDS);
                }   
            
            if (StructureCode == StructureCode.HETROCYCLIC) _categories.Add(HTSCategory.X_ORGANOINORGANICS_HETEROCYCLES);
        }
        else
        {
            Console.WriteLine("[ERROR] Program currently does not support PNs with Polyvalency code: \"{0}\"", Polyvalency);
            Environment.Exit(2);
        }
    }
    public bool MultipleCategories()
    {
        if (_categories.Count > 1) return true;
        return false;
    }
    public HTSCategory[] GetCategoryArray() => _categories.ToArray();

    public StructureCode GetStructureCode() => StructureCode;

}
