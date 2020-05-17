# ImagedropJS
  
Imagedrop.js  v1.0.0 (2020-04-27)  
Copyright 2020 yuchan.  
Licensed under the MIT license.  
  
Description(日本語) :  
画像ファイルを「ドラッグアンドドロップ」または「ダブルクリックしてファイル選択」して、アップロードします。  
Native javascript(ECMAScript 5th edition)で書いてます。jqueryは使用していません。  
既存のドラッグアンドドロップをアップロードするjavascriptライブラリが使いにくかったので作成しました。  
  
高機能ではありませんが、動きとしてはシンプルに作成しています。  
   
    
Set html項のフォーム設置とImagedrop.jsから、下記のテンプレートを自動生成します。  

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

Description(English) :  
Image file "drag and drop" or "double click select" upload. Write native javascript(ECMAScript 5th edition).No jquery.  
  
Support browser : Internet Explorer 11, Google Chrome.  
  
### Set html(Normal)
    <form action="/(Name)/Upload", class="imagedrop", id="imagedrop", enctype="multipart/form-data" />
        <!-- Only when needed → --><input type="number", id="testId", name="testId", style="display: none" />
    </form>


### Set html(ASP.NET MVC5)
    @using (Html.BeginForm("Upload", "(Name)", FormMethod.Post, new { @class = "imagedrop", @id = "imagedrop", enctype = "multipart/form-data" })) {
        @Html.AntiForgeryToken()        
        <!-- Only when needed → -->@Html.Editor("testId", new { htmlAttributes = new { @id = "testId", name = "testId", style="display: none" } })
    }

### Set javascript
The minimum code is as follows.  

    <script>
        var imagedrop = new Imagedrop();
        var filePath = "/Content/images/1/FileUploads/test.jpg"
        imagedrop.setImage(filePath);
    </script>
    
### Server side(Controller)
The following settings are required on the server again.
  
Controller argument: { object file, long testId }  
※For ASP.NET MVC5: { HttpPostedFileBase file, long testId }  
               
Return Json result: new { filePath = "/Content/images/1/FileUploads/test.jpg" }
