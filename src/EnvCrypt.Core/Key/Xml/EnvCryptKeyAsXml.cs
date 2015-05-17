﻿using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
using EnvCrypt.Core.EncryptionAlgo.Aes.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;

namespace EnvCrypt.Core.Key.Xml
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class EnvCryptKey : 
        IKeyExternalRepresentation<AesKey>, 
        IKeyExternalRepresentation<RsaKey>
    {

        private EnvCryptKeyAes[] aesField;

        private EnvCryptKeyRsa[] rsaField;

        private string nameField;

        private string encryptionField;

        private string typeField;

        /// <remarks/>
        [XmlElement("Aes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public EnvCryptKeyAes[] Aes
        {
            get
            {
                return this.aesField;
            }
            set
            {
                this.aesField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Rsa", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public EnvCryptKeyRsa[] Rsa
        {
            get
            {
                return this.rsaField;
            }
            set
            {
                this.rsaField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Encryption
        {
            get
            {
                return this.encryptionField;
            }
            set
            {
                this.encryptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class EnvCryptKeyAes
    {

        private string keyField;

        private string ivField;

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Iv
        {
            get
            {
                return this.ivField;
            }
            set
            {
                this.ivField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class EnvCryptKeyRsa
    {

        private string exponentField;

        private string modulusField;

        private string pField;

        private string qField;

        private string dpField;

        private string dqField;

        private string inverseQField;

        private string dField;

        private bool oaepPaddingField;

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Exponent
        {
            get
            {
                return this.exponentField;
            }
            set
            {
                this.exponentField = value;
            }
        }

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Modulus
        {
            get
            {
                return this.modulusField;
            }
            set
            {
                this.modulusField = value;
            }
        }

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string P
        {
            get
            {
                return this.pField;
            }
            set
            {
                this.pField = value;
            }
        }

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Q
        {
            get
            {
                return this.qField;
            }
            set
            {
                this.qField = value;
            }
        }

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Dp
        {
            get
            {
                return this.dpField;
            }
            set
            {
                this.dpField = value;
            }
        }

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Dq
        {
            get
            {
                return this.dqField;
            }
            set
            {
                this.dqField = value;
            }
        }

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string InverseQ
        {
            get
            {
                return this.inverseQField;
            }
            set
            {
                this.inverseQField = value;
            }
        }

        /// <remarks/>
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string D
        {
            get
            {
                return this.dField;
            }
            set
            {
                this.dField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool OaepPadding
        {
            get
            {
                return this.oaepPaddingField;
            }
            set
            {
                this.oaepPaddingField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class NewDataSet
    {

        private EnvCryptKey[] itemsField;

        /// <remarks/>
        [XmlElement("EnvCryptKey")]
        public EnvCryptKey[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }
}
