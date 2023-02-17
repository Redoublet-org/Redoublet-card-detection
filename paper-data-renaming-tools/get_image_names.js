const FOLDER_PATH = "C:\\Users\\thoma\\Desktop\\darknet\\build\\darknet\\x64\\data\\obj"
let fs = require('fs');
let path = require('path')

fs.readdir(FOLDER_PATH, (err, files) => {
    let names = "";
    for (let file_index = 0; file_index < files.length; file_index++) {
        let filename = files[file_index];

        // Skip all non-jpg files
        if (!filename.endsWith(".jpg")) continue;
        
        names += `data/obj/${filename}\n`
        
    }

    fs.writeFile(path.resolve("./train.txt"), names, file_write_err => {
        if (file_write_err) {
          console.error(file_write_err);
        }
        // file written successfully
      });
})
