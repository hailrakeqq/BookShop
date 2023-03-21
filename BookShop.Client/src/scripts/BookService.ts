import type { Book } from '@/types/Book';
import axios from 'axios'

export function getBookCollectionBySellerId(id: number): Promise<Book[]> {
    return axios.get(`http://localhost:5045/api/Book/GetSellerBookCollection/${id}`, {
        headers: {'Content-Type': 'application/json'}
    }).then(response => {
        if(!response.data) 
            throw new Error("No data received from server");
        
        return response.data as Book[];
    }).catch(e => {
        if (e instanceof SyntaxError &&
            e.message === "JSON.parse: unexpected end of data at line 1 column 1 of the JSON data") {
            alert("error");
        }
        return [] as Book[];
    });
}