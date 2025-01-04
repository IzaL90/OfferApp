import { expect } from "@playwright/test";
import { Locator } from "playwright";

export class DeleteComponent {
  public readonly root: Locator;
  public readonly yes: Locator
  public readonly no: Locator


  constructor(root: Locator){
    this.root = root;
    this.yes = this.root.locator("//button[@data-name='bid-delete-action-confirm']")
    this.no = this.root.locator("//button[@data-name='bid-delete-action-cancel']")
  }

  public async isVisible(): Promise<boolean> {
    return this.root.isVisible();
  }
  
  public async expectModalVisible(): Promise<void> {
    await expect(this.yes).toBeVisible()
    await expect(this.no).toBeVisible()
    
  }

  public async clickYes():Promise<void>{
    await this.yes.click()
}

}