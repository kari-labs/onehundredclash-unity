var LibraryOneHundred = {
    GetSavedJWT: function() {
        var jwt = localStorage.getItem("JWT") || "Null"
        var bufferSize = lengthBytesUTF8(jwt) + 1
        var buffer = _malloc(bufferSize)
        stringToUTF8(jwt, buffer, bufferSize)
        return buffer
        // return localStorage.getItem("JWT")
    },
    Redirect: function(url) {
        window.location.href = Pointer_stringify(url)
    }
}

mergeInto(LibraryManager.library, LibraryOneHundred)