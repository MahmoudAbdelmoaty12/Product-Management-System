export interface IProduct {
    entities:
    {
      Id: number,
      name:string,
      description: string,
      price: number,
      images: [],
      createdDate: Date;
      isDeleted: boolean
    }[],
  count: number
}
