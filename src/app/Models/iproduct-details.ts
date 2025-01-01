export interface IProductDetails {
    isSuccess: boolean,
    message:string,
    entity: {
        productsDtos: {
          id: number,
          name:string,
          description: string,
          price: number,
          createdDate: Date;
          images: [],
        },
    }
}
