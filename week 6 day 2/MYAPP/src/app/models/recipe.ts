export class RecipeModel{
    constructor(public id:number=0,public name:string="",public cuisine:string="",public cookTimeMinutes:string="",public ingredients:string[]=[],public image:string=""){}
}