using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MuebleData", menuName = "Scriptable Objects/MuebleData")]
public class MuebleDatabaseOS : ScriptableObject
{
    public List<MuebleData> muebleData;
    public List<MuebleData> GetMueblesByCategoria(CategoriaMueble categoria)
    {
        return muebleData.FindAll(m => m.Categoria == categoria);
    }
    public List<MuebleData> GetMueblesByEstilo(EstiloDecorativo estilo)
    {
        return muebleData.FindAll(m => m.Estilo == estilo);
    }
    public List<MuebleData> GetMueblesByHabitacion(CategoriaHabitacion categoriaHabitacion)
    {
        return muebleData.FindAll(m => m.CategoriaHabitacion == categoriaHabitacion);
    }
}
[Serializable]
public class MuebleData
{
    [SerializeField] private string name;
    [SerializeField] private int id;
    [SerializeField] private Vector2Int size = Vector2Int.one;
    [SerializeField] private GameObject prefab;
    [SerializeField] private CategoriaMueble categoria;
    [SerializeField] private EstiloDecorativo estilo;
    [SerializeField] private CategoriaHabitacion categoriaHabitacion;
    [SerializeField] private int precio;
    [SerializeField] private Sprite imagenPreview;
    public string Name => name;
    public int ID => id;
    public Vector2Int Size => size;
    public GameObject Prefab => prefab;
    public CategoriaMueble Categoria => categoria;
    public EstiloDecorativo Estilo => estilo;
    public CategoriaHabitacion CategoriaHabitacion => categoriaHabitacion;
    public int Precio => precio;
    public Sprite ImagenPreview => imagenPreview;
}