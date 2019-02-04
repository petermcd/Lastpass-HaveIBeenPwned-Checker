# LastPass Have I Been Pwned Checker

With recent breaches, it has become apparent that there is a need to be able to check the password that you have with databases 
such as Have I Been Pwned.

For 1Password customers this is an easy task as they have directly integrated the API into their service, LastPass, unfortunately,
have not done the same. It would, of course, be a very tedious and laborious task to manually check them.

This is where this tool comes in.

This tool allows the import of the LastPass database backup and checks each password using the LastPass API. You can be safe in
the knowledge the password never leaves your PC. The API allows you to submit the first 5 characters of the SHA 1 hashed version
of the password. The API then returns a list of hashed passwords that start with the supplied characters. This tool then compares
these to identify if a match occurs.

I have supplied both the EXE and source code. At present, the EXE is manually uploaded (until I sort that out) but should always
match the current source version. For those who would prefer to compile themselves you can do so using the source code.

## Dependencies

The project has very few dependencies. These are:

* Costura.Fody - This package allows bundling of other packages.
* FileHelpers - This package is used to parse the Lastpass backup CSV file.

## TODO

So far the tool is very basic and I would like to do a lot more to it. These include:

* Multi-threading - At present, the tool GUI locks up when the passwords are being checked.
* Progress bar - It would be handy to see how close to completion the tool is.
* Prettify - The tool is pretty ugly at present and would benefit from a facelift.