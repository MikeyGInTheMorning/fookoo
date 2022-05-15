const firstLetterToUpper = (data) => {

    var splitDelim = data.indexOf(" ") >= 0 ? " " : "_"
    const words = data.split(splitDelim);

    for (let i = 0; i < words.length; i++) {
        words[i] = words[i][0].toUpperCase() + words[i].substr(1);
    }

    return words.join(" ");
}

const hashData = (/** @type {string} */ data) => {
    var hash = 0, i, chr;

    console.log(data)

    if (data.length === 0) return hash;

    data = data + ""
    for (i = 0; i < data.length; i++) {
        chr = data.charCodeAt(i);
        hash = ((hash << 5) - hash) + chr;
        hash |= 0; // Convert to 32bit integer
    }

    return hash;
};

export {
    hashData,
    firstLetterToUpper
}
