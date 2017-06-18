# Open ssl certificate generation

Crate a pair

	openssl req -x509 -nodes -passin pass:myPassword -days 36500 -newkey rsa:2048 -keyout privatekey.key -out certificate.crt

Merge pair in pkcs12

	openssl pkcs12 -export -in certificate.crt -inkey privatekey.key -out certfile.pfx