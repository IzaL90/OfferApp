import { Page, Locator } from "playwright";
import { expect } from '@playwright/test';
import { DeleteComponent } from "../components/DeleteComponent";
import { PublishUnpublishComponent } from "../components/PublishUnpublish";
import { EditComponent } from "../components/EditPage";
import { FormComponent } from "../components/FormComponent";


export class TablePage {
    public readonly page: Page;
    public readonly root: Locator;
    public readonly row:Locator
    public readonly id: Locator;
    public readonly name: Locator;
    public readonly created: Locator;
    public readonly published: Locator;
    public readonly firstPrice: Locator;
    public readonly lastPrice: Locator;
    public readonly action: Locator;
    public readonly editButton: Locator
    public readonly deleteButton:Locator
    public readonly viewButton :Locator
    public readonly publishUnpublish: PublishUnpublishComponent
    public readonly delete:DeleteComponent
    public readonly edit: EditComponent
    public readonly form:FormComponent

    constructor(page: Page) {
        this.page = page;
        this.root = this.page.locator("//table[contains(@class,'table')]");
        this.row= this.page.locator("//tr[contains(@class, 'table-primary')]//td")
        this.id = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Id']")
        this.name = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Name']")
        this.created = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Created']")
        this.published = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Published']")
        this.firstPrice = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='FirstPrice']")
        this.lastPrice = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='LastPrice']")
        this.action = this.page.locator("//table[contains(@class, 'table')]//th[normalize-space(text())='Action']")
        this.editButton = this.page.locator("//span[contains(@class,'oi-pencil')]")
        this.deleteButton = this.page.locator("//span[contains(@class,'oi-trash')]")
        this.viewButton = this.page.locator("//span[contains(@class,'oi-magnifying-glass')]")
        this.delete = new DeleteComponent(this.page.locator("//div[@class='modal-content']"))
        this.publishUnpublish = new PublishUnpublishComponent(this.root)
        this.edit = new EditComponent(this.page)
        this.form = new FormComponent(this.page);
    }

    public getRowLocator(name: string): Locator {
        return this.page.locator(`//tr[contains(@class,"table")]//td[contains(text(), "${name}")]`);
    }

    public async isVisible(): Promise<boolean> {
        return this.root.isVisible();
    }

    public async expectColumns(): Promise<void> {
        await expect(this.id).toBeVisible()
        await expect(this.name).toBeVisible()
        await expect(this.created).toBeVisible()
        await expect(this.published).toBeVisible()
        await expect(this.firstPrice).toBeVisible()
        await expect(this.lastPrice).toBeVisible()
        await expect(this.action).toBeVisible()
    }

    public getEditLocator(name:string): Locator {
        return this.page.locator(`//tr[contains(@class,"table")]//td[contains(text(), "${name}")]//following-sibling::td//span[contains(@class,"oi-pencil")]`)
    }

    public async clickEdit(name:string): Promise<void> {
        await this.getEditLocator(name).click();
    }

    public async clickView(): Promise<void> {
        await this.viewButton.click();
    }
    
    public getDeleteLocator(name:string): Locator {
        return this.root.locator(`//tr[contains(@class,"table")]//td[contains(text(), "${name}")]//following-sibling::td//span[contains(@class,"oi-trash")]`)
      }
      
      public async clickDelete(name:string): Promise<void> {
        await this.getDeleteLocator(name).click();
      }

}

