export interface User {
    id?: number;
    firstName: string;
    lastName: string;
    gender: string;
    birthday: Date;
    location: string;
    email: string;
    username: string;
    password: string;
    dateOfCreation?: Date;
    bio?: string;
    picture?: File;
}