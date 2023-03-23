import jszip from 'jszip';

export function getImagesHashMapFromServer(): Promise<Map<string, string>> {
    return fetch('http://localhost:5045/api/File/GetAllBookCover')
        .then(response => response.arrayBuffer())
        .then(data => {
            return jszip.loadAsync(data);
        })
        .then(zip => {
            const imageMap: Map<string, string> = new Map();
            const promises: Promise<any>[] = [];

            zip.forEach((relativePath, zipEntry) => {
                if (zipEntry.name.endsWith('.jpg') || zipEntry.name.endsWith('.png')) {
                    const promise = zipEntry.async('blob').then(blob => {
                        const fileName = zipEntry.name.split('.')[0];
                        const imageUrl = URL.createObjectURL(blob);
                        imageMap.set(fileName, imageUrl);
                    });
                    promises.push(promise);
                }
            });

            return Promise.all(promises).then(() => {
                return imageMap;
            });
        });
}
export default {getImagesHashMapFromServer}

