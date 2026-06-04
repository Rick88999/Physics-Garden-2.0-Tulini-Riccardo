public enum Operazione
{
    AltezzaSeno,
    AltezzaCoseno,
    AltezzaTan,
    MassaDaInerzia,
    costanteInerzialeX1,
    costanteInerzialeX2,
    costanteInerzialeO,
    InerziaO,
    InerziaX1,
    InerziaX2,
    InerziaX3,
    DensitaX1,
    DensitaArch,
    DensitaO,
    VolumeCubo,
    VolumeSfera,
    VolumeCilindro,
    VolumeX1,
    None
}

public enum UnitaMisura
{
    Metri,
    Centimetri,
    Millimetri,
    Velocita,
    Angolo,
    Kilogrammi,
    Accelerazione,
    Volume,
    VolumeX1,
    DensitaX1,
    DensitaO,
    Inerzia,
    InerziaX2,
    InerziaX3,
    None
}

public enum TipoCard
{
    DataCard,
    FormulaCard
}

public enum TipoLoot
{
    Card,
    Tool,
    Button
}
public enum Event
{
    SpawnTool,
    SpawnDataCard,
    SpawnFormulaCard
}

public enum TipoTool
{
    MassSensor,
    Spawner
}