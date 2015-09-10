JWTTokenCreator
=================
Simple JWT token creator for testing and debugging JWT tokens. Can encode and decode JWT tokens.
Can be invoked from the commandline as follows:

Usage
=================
To encode a token:
JWTTokenCreator.exe 1 "username" "sharedkey"

where
"username" is the username to encode
"sharedkey" is the key to encode the token

If invoked without arguments then the username and sharedkey can be entered on the commandline manually.
The token that is created is copied to the clipboard.

To decode a token:
JWTTokenCreator.exe 2 "jsontoken" "sharedkey"

where
"jsontoken" is the encoded token to decode
"sharedkey" is the key to decode the token (or they key that originally encoded the token)

If invoked without arguments then the JSON token and sharedkey can be entered on the commandline manually.
The token that is decoded is copied to the clipboard.

Notes
=================
The token that is encoded / decoded is copied to the clipboard.







