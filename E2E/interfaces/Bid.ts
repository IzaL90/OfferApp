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