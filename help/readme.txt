 _____                                      
 |  __ \                                     
 | |__) |___ _ __   __ _ _ __ ___   ___ _ __ 
 |  _  // _ \ '_ \ / _` | '_ ` _ \ / _ \ '__|
 | | \ \  __/ | | | (_| | | | | | |  __/ |   
 |_|  \_\___|_| |_|\__,_|_| |_| |_|\___|_|  v1
(c) 2021, Siddharth Barman

How to rename files?
Files are renamed based on the rules specified in a JSON file. The basic 
structure of a rename looks like this:
{
    "Type":<TYPE OF OPERATION>,
    "What":<WHAT TO REPLACE>,
    "With":<WHAT TO REPLACE IT WITH>
}

All rename rules are run on each file in order. The file is renamed only 
after all rules are run on the original file name, each rule transforms 
the file name and passes the transformed name to the next rule. This means
that it is possible for two opposite rules to nullify each others effect.
E.g. the first rule replaces the word 'foo' with 'bar' while the second 
rule replaces the word 'bar' with 'foo'. If a file named 'foo.txt' is 
processed, its name is unchanged. 

The renaming operations are performed consider either the entire filename
which includes the extension or just the filename without the extension.
This is controlled using the -e switch in the utility. If nothing is 
specified, only the filename without the extension is considered. 

Example 1:
The following rule will replace all occurances of the text 
' welcome 12' with nothing, in other words, all occurances of the
text ' welcome 12' will be removed.
[{
    "Type":"Replace",
    "What":{
        "Type":"Literal",
        "Value":" welcome 123"
    },
    "With":{
        "Type":"Literal",
        "Value":"
    }
}]

Example 2:
The following rule will replace all occurances of the text 'dog' 
with 'cat'.
[{
    "Type":"Replace",
    "What":{
        "Type":"Literal",
        "Value":"dog"
    },
    "With":{
        "Type":"Literal",
        "Value":"cat"
    }
}]

Example 3:
You can replace a matching text with text present in the filename.
[{
    "Type": "Replace",
    "What": {
        "Type": "Literal",
        "Value": "foo"
    },
    "With": {
        "Type":"Positional",
	"Position": { 
            "Start": "1",
            "Length": "1"
        }
    }
}]
This rule will rename a file having name 'foo.txt' to 'o.txt'. 
The "Position" directive requires you to specify the start and the 
number of character from the start position.

Example 4:
You can replace text and tranform it using built in functions. 
This rule will convert all occurances of the text 'foo' to upper-case
that is 'FOO'.
[{
    "Type":"Replace",			
    "What":{
        "Type":"Literal",
        "Value":"foo"
    },
    "With":{
        "Type":"Transform",
        "Value":"ucase"
    }
}]
Similarly, there is a function named 'lcase' which will convert the 
matching text to lower-case.

Example 5:
This example is similar to example 3. Here we are replacing a part 
specified using positional information with a literal text.
[{
    "Type":"Replace",
    "What":{
        "Type":"Positional",
            "Position": { 
                "Start": "0",
                "Length": "1"
            }
    },
    "With":{
        "Type":"Literal",
        "Value":"b"
    }
}]
If this rule is run on a file named 'foo.txt', the new name will 
be 'boo.txt'. 

Example 6:
This is simlar to example 3 and example 5. Here we are replacing a 
part of a string with another part of the same string. 
[{
    "Type":"Replace",
    "What":{
        "Type":"Positional",
        "Position": { 
            "Start": "0",
            "Length": "1"
        }
    },
    "With":{
        "Type":"Positional",
        "Position": { 
            "Start": "2",
            "Length": "1"
        }
    }
}]
Running this rule on a file named 'far.txt' will rename it to 'rar.txt'.

Example 7:
This example transforms positional text to upper-case. 
[{
    "Type":"Replace",
	"What":{
	"Type":"Positional",
	"Position": { 
            "Start": "1",
            "Length": "1"
        }
    },
    "With":{
        "Type":"Transform",					
        "Value": "ucase"					
    }
}]
Running this rule on a file named 'foo.txt' will rename it to 'fOo.txt'.

Example 8:
This example replaces a string positionally from the end. This means 
instead of providing the start position which is from the beginning of 
a string, the start position is provided with respect to the end of the 
string.
[
    {
        "Type":"Replace",
        "What":{
            "Type":"Positional",
            "Position": { 
                "Start": "-5",
                "Length": "1"
        }
    },
    "With":{
        "Type":"Literal",
        "Value":"r"
    }
}]
Running this example on a file named 'foo.txt' will rename it to 'for.txt'
and setting the -e flag.
The number -5 is calculated from the end of the full filename which also 
includes the extension.

Example 9:
This example inserts a literal text at the start of the filename. 
[{
    "Type":"Insert",
    "At": "0",
    "What":{
        "Type":"Literal",
        "Value":"foo"
    }
}]
Running this rule on a file named 'bar.txt' will rename it to 'foobar.txt'.
Using this approach you can insert text anywhere in the string. Position 
0 means the start of the string.

Example 10:
This example appends a string at the end of the filename (includes the 
extension).
[{
    "Type":"Append",				
    "What":{
        "Type":"Literal",
        "Value":"bar"
    }
}]
Running this rule on a file named 'foo.txt' will rename it to 'foobar.txt'.
Running the utility is run with -e flag will rename the file to 'foo.txtbar'.

Example 11:
This example is similar to example 10. Instead of appending a string 
literal, it appends a string obtained using positional information.
[{
    "Type":"Append",				
    "What":{
        "Type" : "Positional",
        "Position": { 
            "Start" : "1",
            "Length": "2"
        }
    }
}]
Running this rule on a file named 'foo.txt' will rename it to 'foooo.txt'.
Running it with -e flag will rename the file to 'foo.txtoo'.



