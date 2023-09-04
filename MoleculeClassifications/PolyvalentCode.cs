namespace MoleculeClassifications;

public enum PolyvalentCode
{
    // First digit of SQL PN
    CARBON_ONLY = 1,
    CARBON_OXYGEN = 2,
    CARBON_NITROGEN = 3,
    CARBON_NITROGEN_OXYGEN = 4,
    CARBON_OTHER = 5,
    CARBON_OXYGEN_OTHER = 6,
    CARBON_NITROGEN_OTHER = 7,
    CARBON_NITROGEN_OXYGEN_OTHER = 8,
    ORGANOMETALLIC = 9,
    INNORGANIC = 10, // No monovalent non metals bonded to C

};