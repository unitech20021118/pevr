using System;
using System.Collections.Generic;
using System.Text;

namespace ZenFulcrum.EmbeddedBrowser {

/**
 * Stand-in class for a JavaScript value that can be one of many different types.
 *
 * Bad lookups are safe. That is, if you try to look up something that doesn't exist you will not get an exception,
 * but an "invalid" node. Use Check() if you want an exception on invalid lookups:
 *
 *   var node = JSONNode.Parse(@"{""a"":1}");
 *   node["a"].IsValid == true;
 *   node["bob"].IsValid == false
 *   node["bob"]["foo"].IsValid == false //but doesn't throw an exception
 *   node["a"].Check() //okay
 *   node["bob"].Check() //throw exception
 *
 * Values can be implicitly converted to JSONNodes and back. That means you don't have to use properties like
 * "IntValue" and "StringValue". Simply try to use the node as that type and it will convert to the value
 * if it's that type or return a default value if it isn't:
 *
 *   var node = JSONNode.Parse(@"{""a"":1, ""b"": ""apples""}");
 *   int a = node["a"];
 *   a == 1;
 *   string b = node["b"];
 *   b == "apples";
 *   string str = node["a"];
 *   str == null; //null is the default value for a string.
 *
 * You can also use new JSONNode(value) for many different types, though often it's easier to just assign a
 * value and let it auto-convert.
 *
 * Because they act a little special, use node.IsNull and node.IsValid to check for null and invalid values.
 * Real null still acts like null, though, so use JSONNode.NullNode to create a "null" JSONNode.
 * You can also use JSONNode.InvalidNode to get an invalid JSONNode outright.
 *
 * Note that, while reading blind is safe, assignment is not. Attempting to assign object keys to an integer, for example,
 * will throw an exception. To append to an array, call .Add() or assign to -1. To remove an object key or array element,
 * assign JSONNode.InvalidNode to it.
 *
 */
public class JSONNode {
	/** Parses the given JSON document to a JSONNode. Throws a SerializationException on parse error. */
	public static JSONNode Parse(string json) {
		return JSONParser.Parse(json);
	}

	public static readonly JSONNode InvalidNode = new JSONNode(NodeType.Invalid);
	public static readonly JSONNode NullNode = new JSONNode(NodeType.Null);

	public enum NodeType {
		/** Getting this value would result in undefined or ordinarily throw some type of exception trying to fetch it. */
		Invalid,
		String,
		Number,
		Object,
		Array,
		Bool,
		Null,
	}

	public NodeType _type = NodeType.Invalid;
	private string _stringValue;
	private double _numberValue;
	private Dictionary<string, JSONNode> _objectValue;
	private List<JSONNode> _arrayValue;
	private bool _boolValue;

	public JSONNode(NodeType type = NodeType.Null) {
		this._type = type;
		if (type == NodeType.Object) _objectValue = new Dictionary<string, JSONNode>();
		else if (type == NodeType.Array) _arrayValue = new List<JSONNode>();
	}

	public JSONNode(string value) {
		this._type = NodeType.String;
		_stringValue = value;
	}

	public JSONNode(double value) {
		this._type = NodeType.Number;
		_numberValue = value;
	}

	public JSONNode(Dictionary<string, JSONNode> value) {
		this._type = NodeType.Object;
		_objectValue = value;
	}

	public JSONNode(List<JSONNode> value) {
		this._type = NodeType.Array;
		_arrayValue = value;
	}

	public JSONNode(bool value) {
		this._type = NodeType.Bool;
		_boolValue = value;
	}

	public NodeType Type { get { return _type; } }

	public bool IsValid {
		get { return _type != NodeType.Invalid; }
	}

	/**
	 * Checks if the node is valid. If not, throws an exception.
	 * Returns {this}, which allows you to add this statement inline in you expressions.
	 *
	 * Example:
	 * var node = data["key1"][1].Check();
	 * int val = data["maxSize"].Check()["elements"][3];
	 */
	public JSONNode Check() {
		if (_type == NodeType.Invalid) throw new InvalidJSONNodeException();
		return this;
	}


	public static implicit operator string(JSONNode n) {
		return n._type == NodeType.String ? n._stringValue : null;
	}
	public static implicit operator JSONNode(string v) {
		return new JSONNode(v);
	}

	public static implicit operator int(JSONNode n) {
		return n._type == NodeType.Number ? (int)n._numberValue : 0;
	}
	public static implicit operator JSONNode(int v) {
		return new JSONNode(v);
	}

	public static implicit operator float(JSONNode n) {
		return n._type == NodeType.Number ? (float)n._numberValue : 0;
	}
	public static implicit operator JSONNode(float v) {
		return new JSONNode(v);
	}

	public static implicit operator double(JSONNode n) {
		return n._type == NodeType.Number ? n._numberValue : 0;
	}
	public static implicit operator JSONNode(double v) {
		return new JSONNode(v);
	}

	/**
	 * Setter/getter for keys on an object. All keys are strings.
	 * Assign JSONNode.InvalidValue to delete a key.
	 */
	public JSONNode this[string k] {
		get {
			if (_type == NodeType.Object) {
				JSONNode ret;
				if (_objectValue.TryGetValue(k, out ret)) return ret;
			}
			return InvalidNode;
		}
		set {
			if (_type != NodeType.Object) throw new InvalidJSONNodeException();
			if (value._type == NodeType.Invalid) _objectValue.Remove(k);
			else _objectValue[k] = value;
		}
	}
	public static implicit operator Dictionary<string, JSONNode>(JSONNode n) {
		return n._type == NodeType.Object ? n._objectValue : null;
	}

	/**
	 * Setter/getter for indicies on an array.
	 * Assign JSONNode.InvalidValue to delete a key.
	 * Assign to "-1" to append to the end.
	 */
	public JSONNode this[int idx] {
		get {
			if (_type == NodeType.Array && idx >= 0 && idx < _arrayValue.Count) {
				return _arrayValue[idx];
			}
			return InvalidNode;
		}
		set {
			if (_type != NodeType.Array) throw new InvalidJSONNodeException();

			if (idx == -1) {
				if (value._type == NodeType.Invalid) {
					_arrayValue.RemoveAt(_arrayValue.Count - 1);
				} else {
					_arrayValue.Add(value);
				}
			} else {
				if (value._type == NodeType.Invalid) {
					_arrayValue.RemoveAt(idx);
				} else {
					_arrayValue[idx] = value;
				}
			}
		}
	}
	public static implicit operator List<JSONNode>(JSONNode n) {
		return n._type == NodeType.Array ? n._arrayValue : null;
	}

	/** Adds an items if the node is an array. */
	public void Add(JSONNode item) {
		if (_type != NodeType.Array) throw new InvalidJSONNodeException();
		_arrayValue.Add(item);
	}

	/** If we are an array or object, returns the size, otherwise returns 0. */
	public int Count {
		get {
			switch (_type) {
				case NodeType.Array: return _arrayValue.Count;
				case NodeType.Object: return _objectValue.Count;
				default: return 0;
			}
		}
	}

	/** True if the value of this node is exactly null. */
	public bool IsNull {
		get { return _type == NodeType.Null; }
	}

	public static implicit operator bool(JSONNode n) {
		return n._type == NodeType.Bool ? n._boolValue : false;
	}
	public static implicit operator JSONNode(bool v) {
		return new JSONNode(v);
	}

	/** Returns a native value representation of our value. */
	public object Value {
		get {
			switch (_type) {
				case NodeType.Invalid:
					Check();
					return null;//we don't get here.
				case NodeType.String:
					return _stringValue;
				case NodeType.Number:
					return _numberValue;
				case NodeType.Object:
					return _objectValue;
				case NodeType.Array:
					return _arrayValue;
				case NodeType.Bool:
					return _boolValue;
				case NodeType.Null:
					return null;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	/** Serializes the JSON node and returns a JSON string. */
	public string AsJSON {
		get {
			return JSONParser.Serialize(this);
		}
	}
}

	public class InvalidJSONNodeException : Exception {}
}
