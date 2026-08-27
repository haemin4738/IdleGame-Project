public interface ISaveProvider
{
    void Save(SaveData data);
    SaveData Load();
}
