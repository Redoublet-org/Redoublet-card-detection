xml-to-yolobb.js: 
TOOL OM DE XML BESTANDEN VOOR BOUNDING BOXES VAN DE PAPER OM TE ZETTEN NAAR 
DE VERWACHTE BOUNDING BOX FORMAT VOOR YOLOV4.

Gebruik: 
- Voer path in naar de folder waar alle XML bestanden instaan in de variabele FOLDER_PATH.
- Heb iets van node.js en npm geinstalleerd staan.
- Run `npm i` om alle dependencies te installeren
- Run `node xml-to-yolobb.js` en dan krijg je in dezelfde folder je .txt bounding box outputs.

get_image_names.js: 
TOOL OM ALLE IMAGE NAMES IN EEN BESTAND TE ZETTEN SAMEN MET DE RELATIEVE PATH OM DE TRAINING SET TE MAKEN

Gebruik: 
- Voer path in naar de folder waar alle XML bestanden instaan in de variabele FOLDER_PATH.
- Heb iets van node.js en npm geinstalleerd staan.
- Run `npm i` om alle dependencies te installeren
- Run `node get_image_names.js` en dan krijg je een bestand met hierin je train.txt relatieve image paths. 
  Deze kan vervolgens gebruikt worden als input voor YOLOV4.