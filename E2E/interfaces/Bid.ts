export interface Bid {
    name: string;
    price: string;
    description: string;
  }

  export const defaultBid: Bid = {
    name: "TestBid",
    price: "7657",
    description: "Default description",
  };

  export const editBid: Bid = {
    name: "EditedBid",
    price: "9999",
    description: "Edited description",
};