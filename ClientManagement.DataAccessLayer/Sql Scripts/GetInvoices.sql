select inv.Id InvoiceId, inv.DueDate InvoiceDueDate, p.Name ProductName,p.Price CurrentProductPrice, inP.ProductAmount, c.CompanyName ClientName  from Invoices inv
left join Clients c 
on inv.ClientId = c.Id
left join InvoicesProducts inP
on inv.Id = inP.InvoiceId
left join Products p
on inP.ProductId = p.Id

select * from Invoices;