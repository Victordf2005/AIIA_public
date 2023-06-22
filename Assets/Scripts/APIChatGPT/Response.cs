using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Response {
    public string id;
    public string objeto;
    public int created;
    public string model;
    public Usage usage;
    public Choices[] choices;
}
