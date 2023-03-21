export interface Book {
    id: string;
    sellerId: string;
    title: string;
    author: string;
    genre: string[];
    description: string;
    price: number;
    yearOfPublication: number;
    countOfPages: number;
    countInStock: number;
    rating: number;
}