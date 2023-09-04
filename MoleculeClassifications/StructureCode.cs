namespace MoleculeClassifications;

public enum StructureCode
{
    // Second digit of SQL PN
    NONCARBONACEOUS,
    ALKANES,
    CYCLOALKANES,
    ALKENES,
    CYCLOALKENES,
    ALKYNES, // These can be cylcic
    AROMATICS,
    ARYL_ALKANES,
    ARYL_TRIFLUOROMETHYL,
    ARYL_ALKENES_ALKYNES,
    HETROCYCLIC, // 4 OR MORE ATOMS IN RING
}