const CLASS_LIST = ["Ah","Kh","Qh","Jh","10h","9h","8h","7h","6h","5h","4h","3h","2h","Ad","Kd","Qd","Jd","10d","9d","8d","7d","6d","5d","4d","3d","2d","Ac","Kc","Qc","Jc","10c","9c","8c","7c","6c","5c","4c","3c","2c","As","Ks","Qs","Js","10s","9s","8s","7s","6s","5s","4s","3s","2s"]
const FOLDER_PATH = "C:\\Users\\Desktop\\Desktop\\Redoublet-card-detection\\data\\paper-test"
let fs = require('graceful-fs');
let parser = require('xml2json');

// Get list of all xml files in the given path 

fs.readdir(FOLDER_PATH, (err, files) => {
    for (let file_index = 0; file_index < files.length; file_index++) {
        let filename = files[file_index];

        // Skip all non-xml files
        if (!filename.endsWith(".xml")) continue;
        
        // For each xml file, convert it to the .txt format
        console.log(filename)
        convertXML(filename)
    }
})

function convertXML(filename) {
    let data = fs.readFileSync( `${FOLDER_PATH}\\${filename}`)
    
    let json_from_xml = JSON.parse(parser.toJson(data));
    let boundingbox_list = json_from_xml.annotation.object;
    let img_width  = json_from_xml.annotation.size.width;
    let img_height = json_from_xml.annotation.size.height;

    // Accumulator for all of the different objects in the file
    let output_lines = ""; 

    for (let i = 0; i < boundingbox_list.length; i++) {
        let obj = boundingbox_list[i]; // Actual object with name and boundingbox
        let class_index = CLASS_LIST.indexOf(obj.name);
        let width  = parseInt(obj.bndbox.xmax) - parseInt(obj.bndbox.xmin);
        let height = parseInt(obj.bndbox.ymax) - parseInt(obj.bndbox.ymin);
        let midX = parseInt(obj.bndbox.xmin) + width / 2;
        let midY = parseInt(obj.bndbox.ymin) + height / 2;
        // console.log(midX);
        
        output_lines += `${class_index} ${midX / img_width} ${midY / img_height} ${width / img_width} ${height / img_height}\n`
    }

    console.log(`Converted ${filename}`)
    fs.writeFileSync(`${FOLDER_PATH}\\${filename.slice(0, -4)}.txt`, output_lines)
     
}
