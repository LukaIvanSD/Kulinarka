export interface UserAchievement{
 achievement:Achievement
 aquiredDate?:Date,
 pointsCollected:Number
}
export interface Achievement{
    name: String,
    description: String,
    requirementType: String,
    pointsNeeded: Number,
    iconBase64: String
}