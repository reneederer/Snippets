﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.7.2558.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.2558.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class ProgramData {
    
    private ProgramDataConfiguration configurationField;
    
    private ProgramDataSnippet[] snippetField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ProgramDataConfiguration Configuration {
        get {
            return this.configurationField;
        }
        set {
            this.configurationField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Snippet", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ProgramDataSnippet[] Snippet {
        get {
            return this.snippetField;
        }
        set {
            this.snippetField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.2558.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ProgramDataConfiguration {
    
    private string defaultWorkingDirectoryField;
    
    private ProgramDataConfigurationInterpreter[] interpreterField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string DefaultWorkingDirectory {
        get {
            return this.defaultWorkingDirectoryField;
        }
        set {
            this.defaultWorkingDirectoryField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Interpreter", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ProgramDataConfigurationInterpreter[] Interpreter {
        get {
            return this.interpreterField;
        }
        set {
            this.interpreterField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.2558.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ProgramDataConfigurationInterpreter {
    
    private bool defaultField;
    
    private bool defaultFieldSpecified;
    
    private string nameField;
    
    private string pathField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool @default {
        get {
            return this.defaultField;
        }
        set {
            this.defaultField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool defaultSpecified {
        get {
            return this.defaultFieldSpecified;
        }
        set {
            this.defaultFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string path {
        get {
            return this.pathField;
        }
        set {
            this.pathField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.2558.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ProgramDataSnippet {
    
    private string categoryField;
    
    private string descriptionField;
    
    private string commandTextField;
    
    private string workingDirectoryField;
    
    private string interpreterField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Category {
        get {
            return this.categoryField;
        }
        set {
            this.categoryField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Description {
        get {
            return this.descriptionField;
        }
        set {
            this.descriptionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string CommandText {
        get {
            return this.commandTextField;
        }
        set {
            this.commandTextField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string workingDirectory {
        get {
            return this.workingDirectoryField;
        }
        set {
            this.workingDirectoryField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Interpreter {
        get {
            return this.interpreterField;
        }
        set {
            this.interpreterField = value;
        }
    }
}