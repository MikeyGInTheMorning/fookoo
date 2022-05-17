const firstLetterToUpper = (data) => {

    var splitDelim = data.indexOf(" ") >= 0 ? " " : "_"
    const words = data.split(splitDelim);

    for (let i = 0; i < words.length; i++) {
        words[i] = words[i][0].toUpperCase() + words[i].substr(1);
    }

    return words.join(" ");
}

const cleanName = (name) => {
    var newName = firstLetterToUpper(name)
    return newName.replace(' ', '_')
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

const addSaying = async (sentence) => {

    const body = {
        sentence,
    };
    return await fetch('http://localhost:7071/api/AddSaying', {
        method: 'POST',
        headers: {
            Accept: 'application/json',
        },
        body: JSON.stringify(body)
    })
        .then(response => response.json())
        .then((res) => { return res; })
        .catch((e) => {
            console.error(e);
            return false;
        });
}

const getSaying = async (hash) => {
    return await fetch(`http://localhost:7071/api/GetSaying?hash=${hash}`, {
        headers: {
            'Content-Type': 'application/json'
        },
    })
        .then(response => response.json())
        .then(res => {
            console.log(res)
            return res.saying.sentence
        })
        .catch(e => {
            console.error(e)
            return null
        })
}

export {
    hashData,
    firstLetterToUpper,
    addSaying,
    getSaying,
    cleanName
}
