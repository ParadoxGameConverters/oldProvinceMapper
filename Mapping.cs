﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProvinceMapper
{
	interface Mapping
	{
		bool isIncomplete();
		string ToString();
		string ToOutputString(string srcTag, string destTag);
	}

	class ProvinceMapping : Mapping
	{
		public List<Province> srcProvs = new List<Province>();
		public List<Province> destProvs = new List<Province>();
		public List<string> resettableRegions = new List<string>();

		public ProvinceMapping()
		{
		}

		public ProvinceMapping(string line, string srcTag, string destTag, List<Province> possibleSources, List<Province> possibleDests, string currentMapping)
		{
			string[] tokens = line.Split(' ', '\t');
			int parseMode = 0;
			foreach (string s in tokens)
			{
				if (s == "=" || s == String.Empty)
				{
					continue;
				}
				else if (s == "link" || s == "{" || s == "}" || s == "=")
				{
					parseMode = 0;
				}
				else if (s == srcTag)
				{
					parseMode = 1;
				}
				else if (s == destTag)
				{
					parseMode = 2;
				}
				else if (s == "resettable")
				{
					parseMode = 3;
				}
				else if (s[0] == '#')
				{
					break;
				}
				else
				{
					switch (parseMode)
					{
						case 1:
							{
								// provID is src
								int provID = int.Parse(s.Trim('}'));
								Province prov = possibleSources.Find(
									 delegate (Province p)
									 {
										 return p.ID == provID;
									 });
								if (prov == null)
									throw new Exception(String.Format("Province \"{0}\" appears in a mapping, but not in game data!", prov.ToString()));
								if (prov.mappings.ContainsKey(currentMapping))
									throw new Exception(String.Format("Province \"{0}\" appears in more than one mapping!", prov.ToString()));
								prov.mappings.Add(currentMapping, this);
								srcProvs.Add(prov);
								break;
							}
						case 2:
							{
								// provID is dest
								int provID = int.Parse(s.Trim('}'));
								Province prov = possibleDests.Find(
									 delegate (Province p)
									 {
										 return p.ID == provID;
									 });
								if (prov == null)
								{
									throw new Exception(String.Format("Province \"{0}\" appears in a mapping, but not in game data!", prov.ToString()));
								}
								if (prov.mappings.ContainsKey(currentMapping))
								{
									throw new Exception(String.Format("Province \"{0}\" appears in more than one mapping!", prov.ToString()));
								}
								prov.mappings.Add(currentMapping, this);
								destProvs.Add(prov);
								break;
							}
						case 3:
							{
								string region = s.Trim('}');
								resettableRegions.Add(region);
								break;
							}
						default:
							throw new Exception(String.Format("Unexpected token {0}", s));
					}

					parseMode = 0;
				}
			}
		}

		public override string ToString()
		{
			string comma = String.Empty;
			string retval = String.Empty;
			foreach (Province p in srcProvs)
			{
				retval += comma;
				retval += p.name;
				comma = ", ";
			}
			comma = String.Empty;
			retval += " -> ";
			foreach (Province p in destProvs)
			{
				retval += comma;
				retval += p.name;
				comma = ", ";
			}
			return retval;
		}

		public virtual string ToOutputString(string srcTag, string destTag)
		{
			if (srcProvs.Count == 0 && destProvs.Count == 0)
			{
				return "";
			}
			string retval = String.Empty;
			retval += "\tlink = { ";
			foreach (Province p in srcProvs)
			{
				retval += srcTag + " = " + p.ID.ToString() + " ";
			}
			foreach (Province p in destProvs)
			{
				retval += destTag + " = " + p.ID.ToString() + " ";
			}
			foreach (string r in resettableRegions)
			{
				retval += "resettable = " + r + " ";
			}
			retval += "}\t# ";
			if (isManyToMany())
			{
				retval += "MANY-TO-MANY: ";
			}
			if (srcProvs.Count == 0)
			{
				retval += "NOTHING";
			}
			retval += this.ToString();
			if (destProvs.Count == 0)
			{
				retval += "DROPPED";
			}
			return retval;
		}

		public bool isManyToMany()
		{
			return (srcProvs.Count > 1) && (destProvs.Count > 1);
		}

		public virtual bool isIncomplete()
		{
			return (srcProvs.Count == 0) || (destProvs.Count == 0);
		}

		public bool isInvalid()
		{
			return isManyToMany() || isIncomplete();
		}
	}

	class CommentMapping : Mapping
	{
		public string commentLine;

		public CommentMapping()
		{
			commentLine = String.Empty;
		}

		public CommentMapping(string line)
		{
			if (line == String.Empty)
			{
				commentLine = String.Empty;
			}

			string tmpStr = line.Remove(0, 1);
			commentLine = tmpStr.Trim();
		}

		public bool isIncomplete()
		{
			return false; // comments are always complete
		}

		public override string ToString()
		{
			return "# " + commentLine;
		}

		public string ToOutputString(string srcTag, string destTag)
		{
			return "\t# " + commentLine;
		}
	}
}
