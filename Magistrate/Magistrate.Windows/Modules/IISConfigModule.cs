using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Magistrate.Windows.Modules
{
    internal sealed class IISConfigModule : BaseModule
    {
        private bool DebugVerbose = false;
        private const string IISLocation = @"C:\Windows\sysnative\inetsrv\Config\applicationHost.config";
        public IISConfigModule() : base()
        {
            SetTickRate(28069);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            if (!File.Exists(IISLocation))
                return SingleState(CheckInfo.DEFAULT);

            string rootName = info.GetArgument(0)?.ToString();
            List<string> props = info.GetArgument(1)?.ToString().Split(';').ToList();

            if (rootName == null || props == null)
                return SingleState(CheckInfo.DEFAULT);

            XmlDocument conf = new XmlDocument();
            conf.LoadXml(File.ReadAllText(IISLocation));

            var AllRoots = conf.GetElementsByTagName(rootName);

            List<CheckState> states = new List<CheckState>();

            List<XmlNode> AllNodes = new List<XmlNode>();

            foreach (XmlNode node in AllRoots)
                AllNodes.Add(node);

            var all = FindNodesForCollection(AllNodes);

            foreach (XmlNode node in all)
            {
                if (node.Name.StartsWith("#"))
                    continue;

                if (node.Attributes == null)
                    continue;

                try
                {
                    foreach(XmlAttribute attrib in node.Attributes)
                    {
                        if (!props.Contains(attrib.Name))
                            continue;

                        string sstr = $"{node.Name.ToLower()}:{attrib.Name.ToLower()}:{attrib.Value?.ToLower() ?? ""}";
#if DEBUG
                        if (DebugVerbose)
                            Console.WriteLine(sstr);
#endif
                        states.Add(new CheckState(info.ComputeHash(sstr)));
                    }
                }
                catch(Exception e)
                {
#if DEBUG
                    if (DebugVerbose)
                        Console.WriteLine(e.ToString());
#endif
                }


            }

            return states;
        }

        private List<XmlNode> FindNodesForCollection(List<XmlNode> prevnodes)
        {
            List<XmlNode> Children = new List<XmlNode>();

            foreach(var node in prevnodes)
            {
                if (!node.HasChildNodes)
                    continue;

                foreach (XmlNode c in node.ChildNodes)
                    Children.Add(c);
            }

            if(Children.Count > 0)
                Children.AddRange(FindNodesForCollection(Children));

            Children.AddRange(prevnodes);
            return Children;
        }
    }
}
