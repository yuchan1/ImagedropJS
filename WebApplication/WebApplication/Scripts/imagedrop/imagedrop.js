/*
 * Imagedrop.js  v1.0.0 (2020-04-27)
 * Copyright 2020 yuchan.
 * Licensed under the MIT license.
 *
 * Description     : Image file drag and drop upload. Write native javascript(No jquery). ECMAScript 5th edition.
 * Support browser : Internet Explorer 11, Google Chrome.
 *
 * How to use
 * ・Set html
 *   @using (Html.BeginForm("Upload", "(Name)", FormMethod.Post, new { @class = "imagedrop", @id = "imagedrop", enctype = "multipart/form-data" })) {
 *       @Html.AntiForgeryToken()        
 *       <!-- Only when needed → -->@Html.Editor("testId", new { htmlAttributes = new { @id = "testId", name = "testId", style="display: none" } })     
 *   }
 *
 *   or
 *   <form action="/(Name)/Upload", class="imagedrop", id="imagedrop", enctype="multipart/form-data" />
 *       <!-- Only when needed → --><input type="number", id="testId", name="testId", style="display: none" />
 *   </form>
 * 
 * ・javascript
 *   <script>
 *       var imagedrop = new Imagedrop();
 *       var filePath = "/Content/images/1/test.jpg"
 *       imagedrop.setImage(filePath);
 *   </script>
 *
 *   <!-- Upload the file by dragging and dropping the image file or selecting it by double clicking. -->
 *
 * ・Server side
 *  Do     : Html post form data. { object[] files, long testId}
 *  Return : Json result required. { filePath = "/Content/images/1/test.jpg" }
 */

var Imagedrop = (function () {

    // Constractor
    var Imagedrop = function () {
        this.init();
    }

    // Method
    Imagedrop.prototype.init = function () {
        var form = document.getElementById("imagedrop");
        var dragAndDropArea = document.getElementById("dragAndDropArea");

        var template = "\n" +
            "<div class=\"drag-and-drop-area\" id=\"dragAndDropArea\">\n" +
            "    <input type=\"file\" id=\"file\" name=\"file\" accept=\"image/jpeg\" style=\"display: none\" />\n" +
            "    <div class=\"default-message\" id=\"defaultMessage\">\n" +
            "        <p>ここに画像ファイルをドラッグ＆ドロップ、またはダブルクリックして選択</p>\n" +
            "        <p>(.jpgファイル、1MBまで)</p>\n" +
            "    </div>\n" +
            "    <div class=\"preview-image\" id=\"previewImage\" style=\"display: none\"></div>\n" +
            "    <div class=\"preview-filename\" id=\"previewFileName\" style=\"display: none\"></div>\n" +
            "</div>\n";

        dragAndDropArea ? dragAndDropArea.parentNode.removeChild(dragAndDropArea) : null;
        form.insertAdjacentHTML("afterbegin", template);

        var _self = this;

        this._form = document.getElementById("imagedrop");
        this._dragAndDropArea = document.getElementById("dragAndDropArea");
        this._inputFile = document.getElementById("file");
        this._defaultMessage = document.getElementById("defaultMessage");
        this._previewImage = document.getElementById("previewImage");
        this._previewFileName = document.getElementById("previewFileName");
        this._files = null;

        this._dragAndDropArea.addEventListener("dragenter", function (event) {
            event.stopPropagation();
            event.preventDefault();
        });

        this._dragAndDropArea.addEventListener("dragover", function (event) {
            event.stopPropagation();
            event.preventDefault();
            event.dataTransfer.dropEffect = "copy"
        });

        this._dragAndDropArea.addEventListener("drop", function (event) {
            event.stopPropagation();
            event.preventDefault();

            // Support Internet Explorer 11
            _self._files = event.dataTransfer.files;

            if (_self._previewImage.innerHTML) {
                alert("ファイルがアップロード済みのため実行できません。");
                return;
            }

            if (_self.updateCheck()) {
                _self.upload(true);
            }
        });

        this._dragAndDropArea.addEventListener("dblclick", function (event) {
            if (_self._previewImage.innerHTML) {
                alert("ファイルがアップロード済みのため実行できません。");
                return;
            }

            _self._inputFile.click();
        });

        this._inputFile.addEventListener("change", function (event) {
            _self._files = _self._inputFile.files;

            if (_self.updateCheck()) {
                _self.upload(false);
            }
        });

        this._dragAndDropArea.addEventListener("dragenter", function (event) {
            event.stopPropagation();
            event.preventDefault();
        });

        this._dragAndDropArea.addEventListener("dragover", function (event) {
            event.stopPropagation();
            event.preventDefault();
        });

        this._dragAndDropArea.addEventListener("drop", function (event) {
            event.stopPropagation();
            event.preventDefault();
        });
    }

    Imagedrop.prototype.updateCheck = function () {
        var files = this._files;

        if (!files) {
            alert("ファイルが見つからないためアップロードは実行できません。");
            return false;
        }

        if (files.length > 1) {
            alert("複数ファイルのアップロードは実行できません。");
            return false;
        }
        
        var file = files[0];
        var fileName = file.name.toLowerCase();
        var pos = fileName.lastIndexOf('.');
        var fileExtension = fileName.slice(pos);

        var fileType = file.type;

        if (fileExtension != ".jpg" || fileType != "image/jpeg") {
            alert("jpg以外のファイルはアップロードは実行できません。");
            return false;
        }

        var fileSize = file.size / 1024 / 1024;

        if (fileSize > 1) {
            alert("1MBより大きいサイズのファイルはアップロードは実行できません。");
            return false;
        }

        return true;
    }

    Imagedrop.prototype.upload = function (dragAndDropMode) {
        var _self = this;
        var uploadUrl = this._form.action;
      
        if (dragAndDropMode) {
            var files = this._files;
            this._inputFile.parentNode.removeChild(this._inputFile);

            var formData = new FormData(this._form);
            var file = files[0];
            formData.append("file", file, file.name);      
        } else {
            var formData = new FormData(this._form);
        }

        var xhr = new XMLHttpRequest();
        xhr.open("POST", uploadUrl, true);
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status == 200) {
                    var filePath = JSON.parse(xhr.responseText).filePath
                    _self.setImage(filePath);
                    xhr.abort();
                    console.log("success");
                } else {
                    _self.init();
                    _self.setImage("");
                    var message = JSON.parse(xhr.responseText).Message;
                    xhr.abort();
                    console.log("failure : " + message);
                    alert(message);
                }
            }
        }
    }

    Imagedrop.prototype.setImage = function (filePath) {
        var image = document.getElementById("image");

        // If "filePath" is not include "fileName"
        var pattern = "[^/]+$";
        var match = filePath.match(pattern);

        if (!match || !filePath) {
            // Form init
            this.init();

            this._defaultMessage.style.display = "block";
            this._previewImage.style.display = "none";
            this._previewFileName.innerHTML = "";

            return;
        }

        var fileName = match[0];

        // Check exist file path 
        var xhr = new XMLHttpRequest();
        xhr.open("HEAD", filePath, false);  // async false is not recommended.
        xhr.setRequestHeader("Pragma", "no-cache");
        xhr.setRequestHeader("Cache-Control", "no-cache");
        xhr.send(null);

        var status = false;
        if (xhr.status == 200) {
            status = true;
        }
        xhr.abort();
        
        // Form init
        this.init();

        // Image insert or remove
        if (status) {
            var imageHtml = "<img class=\"image\" id=\"image\" src=\"" + filePath + "\" />"
            this._previewImage.insertAdjacentHTML("beforeend", imageHtml);

            this._defaultMessage.style.display = "none";
            this._previewImage.style.display = "block";
            this._previewFileName.innerHTML = fileName;

        } else {
            this._defaultMessage.style.display = "block";
            this._previewImage.style.display = "none";
            this._previewFileName.innerHTML = "";
        }

        // Fire window resize event
        var resizeEvent = window.document.createEvent("UIEvents");
        resizeEvent.initUIEvent("resize", true, false, window, 0);
        window.dispatchEvent(resizeEvent);
        // window.dispatchEvent(new Event("resize"));
    }

    Imagedrop.prototype.setHeight = function (height) {
        this._dragAndDropArea.style.minHeight = height + "px";
        this._dragAndDropArea.style.maxHeight = height + "px";
    }

    Imagedrop.prototype.getFileName = function () {
        return this._previewFileName.innerHTML;
    }

    return Imagedrop;
})();