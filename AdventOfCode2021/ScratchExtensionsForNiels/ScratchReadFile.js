'use strict';
class ScratchReadFile {
    constructor() {
    }

    getInfo() {
        return {
            "id": "ReadFile",
            "name": "OpenFile",
            "blocks": [
                {
                    "opcode": "readAll",
                    "blockType": "reporter",
                    "text": "read data from [file]",
                    "arguments": {
                        "file": {
                            "type": "string",
                            "defaultValue": "c:/temp/input.txt"
                        },
                    }
                },
            ]
        }
    }

    /* add methods for blocks */
    readAll({ file }) {
        const fs = require('fs');
        fs.readFile(file, (err, data) => {
            if (err) return err.toString();
            return data.toString();
        })
    }
}

Scratch.extensions.register(new ScratchReadFile())