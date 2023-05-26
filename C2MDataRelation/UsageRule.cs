﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace C2MDataRelation
{
    class UsageRule
    {
        String usageGroup;
        String name;
        int sequence;
        String referredUsageGroup;
        String usageRuleType;
        String boDataArea;
        List<EligibilityCriteria> eligibilityCriteria;
        String schema;
        List<sq> sqList;
        public UsageRule(string usageGroup, string name, int sequence, string usageRuleType, string boDataArea, string referredUsageGroup, List<EligibilityCriteria> eligibilityCriteria)
        {
            this.usageGroup = usageGroup;
            this.Name = name;
            this.Sequence = sequence;
            this.ReferredUsageGroup = referredUsageGroup;
            this.UsageRuleType = usageRuleType;
            this.BoDataArea = boDataArea;
            this.EligibilityCriteria = eligibilityCriteria;
        }
        public UsageRule(OracleDataReader orr)
        {
            this.usageGroup = orr.GetString(orr.GetOrdinal("USG_GRP_CD"));
            this.name = orr.GetString(orr.GetOrdinal("USG_RULE_CD"));
            this.Sequence = orr.GetInt16(orr.GetOrdinal("EXE_SEQ"));
            this.ReferredUsageGroup = orr.GetString(orr.GetOrdinal("REFERRED_USG_GRP_CD"));
            this.UsageRuleType = orr.GetString(orr.GetOrdinal("BUS_OBJ_CD"));
            this.BoDataArea = "<" + this.name + ">\n" + orr.GetString(orr.GetOrdinal("BO_DATA_AREA")) + "</" + this.name + ">";
            this.EligibilityCriteria = new List<EligibilityCriteria>();
            this.SqList = new List<sq>();
        }

        public string Name { get => name; set => name = value; }
        public int Sequence { get => sequence; set => sequence = value; }
        public string UsageRuleType { get => usageRuleType; set => usageRuleType = value; }
        public string BoDataArea { get => boDataArea; set => boDataArea = value; }
        public string ReferredUsageGroup { get => referredUsageGroup; set => referredUsageGroup = value; }
        internal List<EligibilityCriteria> EligibilityCriteria { get => eligibilityCriteria; set => eligibilityCriteria = value; }
        public string UsageGroup { get => usageGroup; set => usageGroup = value; }
        public string Schema { get => schema; set => schema = value; }
        internal List<sq> SqList { get => sqList; set => sqList = value; }

        public void addSq(sq SQ)
        {
            this.SqList.Add(SQ);
        }
        public void addEligibilityCriteria(EligibilityCriteria eligibilityCriteria) {
            this.eligibilityCriteria.Add(eligibilityCriteria);
        }
        public void getSq() {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(this.boDataArea);
            XmlNodeList uomList = xd.GetElementsByTagName("uom");
            XmlNodeList sqiList = xd.GetElementsByTagName("sqi");
            XmlNodeList touList = xd.GetElementsByTagName("tou");
            Dictionary<sq, int> filterMap = new Dictionary<sq, int>();
            //needs jesus
            for (int i = 0; i < uomList.Count; i++)
            {
                XmlNode parent = uomList[i].ParentNode;
                for (int j = 0; j < parent.ChildNodes.Count; j++) {
                    sq tempSQ = new sq();
                    if (parent.ChildNodes[j].NodeType == XmlNodeType.Element)
                    {
                        if (parent.ChildNodes[j].Name.Equals("uom")) {
                            tempSQ.Uom = parent.ChildNodes[j].InnerText;
                        }
                        if (parent.ChildNodes[j].Name.Equals("sqi"))
                        {
                            tempSQ.Sqi = parent.ChildNodes[j].InnerText;
                        }
                        if (parent.ChildNodes[j].Name.Equals("tou"))
                        {
                            tempSQ.Tou = parent.ChildNodes[j].InnerText;
                        }
                    }
                    if (tempSQ.Uom == null && tempSQ.Sqi == null && tempSQ.Tou == null)
                    {

                    }
                    else {
                        if (!filterMap.ContainsKey(tempSQ))
                        {
                            filterMap.Add(tempSQ, 1);
                        }
                    }
                    
                   
                }
            }
            for (int i = 0; i < sqiList.Count; i++)
            {
                XmlNode parent = sqiList[i].ParentNode;
                for (int j = 0; j < parent.ChildNodes.Count; j++)
                {
                    sq tempSQ = new sq();
                    if (parent.ChildNodes[j].NodeType == XmlNodeType.Element)
                    {
                        if (parent.ChildNodes[j].Name.Equals("uom"))
                        {
                            tempSQ.Uom = parent.ChildNodes[j].InnerText;
                        }
                        if (parent.ChildNodes[j].Name.Equals("sqi"))
                        {
                            tempSQ.Sqi = parent.ChildNodes[j].InnerText;
                        }
                        if (parent.ChildNodes[j].Name.Equals("tou"))
                        {
                            tempSQ.Tou = parent.ChildNodes[j].InnerText;
                        }
                    }
                    if (tempSQ.Uom == null && tempSQ.Sqi == null && tempSQ.Tou == null)
                    {

                    }
                    else
                    {
                        if (!filterMap.ContainsKey(tempSQ))
                        {
                            filterMap.Add(tempSQ, 1);
                        }
                    }


                }
            }
            for (int i = 0; i < touList.Count; i++)
            {
                XmlNode parent = touList[i].ParentNode;
                for (int j = 0; j < parent.ChildNodes.Count; j++)
                {
                    sq tempSQ = new sq();
                    if (parent.ChildNodes[j].NodeType == XmlNodeType.Element)
                    {
                        if (parent.ChildNodes[j].Name.Equals("uom"))
                        {
                            tempSQ.Uom = parent.ChildNodes[j].InnerText;
                        }
                        if (parent.ChildNodes[j].Name.Equals("sqi"))
                        {
                            tempSQ.Sqi = parent.ChildNodes[j].InnerText;
                        }
                        if (parent.ChildNodes[j].Name.Equals("tou"))
                        {
                            tempSQ.Tou = parent.ChildNodes[j].InnerText;
                        }
                    }
                    if (tempSQ.Uom == null && tempSQ.Sqi == null && tempSQ.Tou == null)
                    {

                    }
                    else
                    {
                        if (!filterMap.ContainsKey(tempSQ))
                        {
                            filterMap.Add(tempSQ, 1);
                        }
                    }


                }
            }
            foreach (KeyValuePair<sq, int> entry in filterMap)
            {
                this.SqList.Add(entry.Key);
            }


        }
        public void combineSchemaAndBoDataArea() {
            String result="";
            if (this.schema != null && this.boDataArea != null)
            {
                Dictionary<String, String> nodes = new Dictionary<string, string>();

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(this.boDataArea);
                XmlNode node = xd.DocumentElement;

                XmlNode xn= node.ChildNodes[0];//initializing only

                XmlNodeList cNode=node.ChildNodes;
                for (int i = 0; i < cNode.Count; i++) {
                    xn = node.ChildNodes[i];
                    if (xn.NodeType == XmlNodeType.Element) {
                        nodes.Add(xn.Name, xn.InnerXml);
                    }
                }
                xd = new XmlDocument();
                xd.LoadXml(this.schema);
                node = xd.DocumentElement;
                cNode = node.ChildNodes;
                for (int i = 0; i < cNode.Count; i++)
                {
                    xn = node.ChildNodes[i];
                    if (xn.NodeType == XmlNodeType.Element)
                    {
                        if (nodes.ContainsKey(xn.Name)) {
                            xn.InnerXml = nodes[xn.Name];
                        }
                    }
                }
                this.schema= xd.OuterXml;
                //asdasd
            }
        }
        public string print() {
            String result ="";
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(this);
                result += String.Format("{0}={1}", name, value) +"\n\n\n";
            }
            return result;
        }
        public string printSQ()
        {
            String result="";
            foreach (sq SQ in this.sqList) {
                result += SQ.print() + "\n";
            }
            return result;
        }
        
    }
}
