using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    [SerializeField]
    private MuebleDatabaseOS muebleDatabase;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;

        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

     
        if (muebleDatabase == null)
        {
            LoadDatabaseFromResources();
        }
    }

    private void LoadDatabaseFromResources()
    {
        muebleDatabase = Resources.Load<MuebleDatabaseOS>("MuebleDatabases/MuebleData");

        if (muebleDatabase == null)
        {
            Debug.LogError("DatabaseManager: MuebleDatabaseOS no encontrada en Resources");
        }
        else
        {
            Debug.Log("DatabaseManager: Base de datos cargada desde Resources");
        }
    }

   
    public MuebleDatabaseOS GetMuebleDatabase()
    {
        if (muebleDatabase == null)
        {
            LoadDatabaseFromResources();
        }
        return muebleDatabase;
    }

    public void SetMuebleDatabase(MuebleDatabaseOS database)
    {
        muebleDatabase = database;
        Debug.Log("DatabaseManager: Base de datos actualizada");
    }
}