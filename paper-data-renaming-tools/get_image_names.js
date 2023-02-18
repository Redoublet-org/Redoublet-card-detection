const FOLDER_PATH = "C:/Users/Desktop/Desktop/Redoublet-card-detection/data/paper-test"
let fs = require('fs');
let path = require('path')

fs.readdir(FOLDER_PATH, (err, files) => {
    let names = "";
    for (let file_index = 0; file_index < files.length; file_index++) {
        let filename = files[file_index];

        // Skip all non-jpg files
        if (!filename.endsWith(".jpg")) continue;
        
        names += `build/darknet/x64/data/obj/${filename}\n`
        
    }

    fs.writeFile(path.resolve("./test.txt"), names, file_write_err => {
        if (file_write_err) {
          console.error(file_write_err);
        }
        // file written successfully
      });
})
