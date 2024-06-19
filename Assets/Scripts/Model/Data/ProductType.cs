using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductType : MonoBehaviour {

    string productTypeName;
    
    public static List< ProductType> productTypeList=new List<ProductType>();

    public ProductType(string name)
    {
        productTypeName = name;
        
    }
}
