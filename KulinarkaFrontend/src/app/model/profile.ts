export interface UserInfo{
    firstName: String,
    lastName: String,
    gender: String,
    birthday: Date,
    location: String,
    email: String,
    username: String,
    bio: String,
    dateOfCreation: Date,
    pictureBase64: String
}
export interface Title{
    name: String,
    aquiredDate: Date,
    numberOfCompletedAchievements: Number,
    nextTitleRequirement: Number,
    currentTitleRequirement:Number,
    nextTitleName: String

}
export interface UserStatistics{
    numberOfLikes: Number,
    numberOfFavorites: Number,
    numberOfRecipes: Number,
    numberOfFollowers: Number,
    averageRating: Number
}
export interface PromotionReward{
    durationInDays: Number,
    intervalInDays: Number,
    postsToPromote: Number
}
export interface Profile {
    userInfo:UserInfo,
    title:Title,
    userStatistics:UserStatistics,
    currentPromotionReward: PromotionReward,
    nextPromotionReward: PromotionReward,
}