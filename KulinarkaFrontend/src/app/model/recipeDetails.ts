export interface Tag{
    id:number,
    tagType:string,
    name:string
}
export interface Ingredient{
    name:string,
    amount:number,
    measurementUnit:string   
}
export interface Recipe{
    id:number,
    userId:number,
    name:string,
    description:string,
    pictureBase64:string,
    videoBase64?:string,
    contentType?:string,
    duration:number,
    difficulty:string,
    numberOfPeople:number,
    chefsAdvice:string,
    creationDate:Date
}
export interface PreparationStep{
    description:string,
    sequenceNumber:number
}
export interface User{
    id:number,
    username:string
    title:string
    pictureBase64: string,
    dateOfCreation: Date
}
export interface CommentWithImage{
    comment:Comment,
    preparedRecipeImage:PreparedRecipeImage
}
export interface Comment{
    id:number,
    text:string,
    postDate:Date,
    header:string,
    creatorName:string,
    creatorTitle:string,
    creatorPictureBase64:string,
    creatorId:number
}
export interface PreparedRecipeImage{
    creatorId:number,
    creatorPictureBase64:string,
    creatorUsername:string,
    dateUploaded:Date,
    imageBase64:string
}
export interface RecipeDetails{
    recipe:Recipe;
    ingredients:Ingredient[];
    steps:PreparationStep[];
    tags:Tag[];
    owner:User;
    comments : Comment[];
    preparedRecipeImages: PreparedRecipeImage[];
    isPromoted:boolean;
}