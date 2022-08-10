using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

public class Rootobject
{
    public Metadata metadata { get; set; }
    public Data data { get; set; }
}

public class Metadata
{
    public string name { get; set; }
    public string catalog_type { get; set; }
    public string catalog_value { get; set; }
}

public class Data
{
    public List<Product> products { get; set; }
}

public class Product
{
    public int id { get; set; }
    public string name { get; set; }
    public string brand { get; set; }
    public int feedbacks { get; set; }
    public double priceU { get; set; }
}


