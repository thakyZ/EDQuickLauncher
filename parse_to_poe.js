var fs = require("fs");
const config = require('./EDQuickLauncher_Localizable.json');

var run = function() {
  var json_data = JSON.parse(fs.readFileSync("./EDQuickLauncher_Localizable.json", "utf-8"));
  var new_data = [];
  for (const [key, value] of Object.entries(json_data)) {
    new_data.push({term: key, definition: value.message, content: "", term_plural: "", reference: value.description, comment: ""});
  }
  fs.writeFileSync("./POEditor_English.json", JSON.stringify(new_data, null, 2), {encoding:"utf-8",flag:"w"});
};

run();