# Paramètres
$ProjectPath = "C:\D\Work\Projets\portefolio\last\Brute_Rar" # Remplacez par le chemin réel de votre projet
$StartDate = Get-Date "2022-06-16" # Date de début des commits
$CommitMessagePrefix = "Commit du " # Préfixe du message de commit
$WaitForKeyPress = $true # Définir à $true pour demander une touche Entrée avant chaque commit
$LoremIpsumContent = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
"@ # Contenu Lorem Ipsum pour les fichiers

# --- NE PAS MODIFIER AU-DELÀ DE CE POINT SAUF SI VOUS SAVEZ CE QUE VOUS FAITES ---

Write-Host "Début de l'automatisation des commits..." -ForegroundColor Yellow

try {
    Set-Location $ProjectPath

    $CurrentDate = $StartDate

    while ($true) {
        # Formater la date pour le message de commit et la variable d'environnement Git
        $FormattedDateForMessage = $CurrentDate.ToString("dd/MM/yyyy")
        $GitDate = $CurrentDate.ToString("yyyy-MM-dd HH:mm:ss") + " +0100" # Heure et fuseau horaire (ajuster si nécessaire)
        $FileName = $CurrentDate.ToString("yyyy-MM-dd") + ".txt" # Nom du fichier avec la date

        Write-Host ""
        Write-Host "Prochain commit pour la date : $FormattedDateForMessage" -ForegroundColor Cyan
        Write-Host "Création du fichier : $FileName" -ForegroundColor Cyan

        if ($WaitForKeyPress) {
            Write-Host "Appuyez sur la touche Entrée pour effectuer le commit (ou Ctrl+C pour quitter)..." -ForegroundColor Green
            $null = Read-Host
        }

        # Créer ou mettre à jour le fichier avec le contenu Lorem Ipsum
        Write-Host "Écriture du contenu dans le fichier $FileName..."
        $Path = Join-Path $ProjectPath "\logs"
        $FilePath = Join-Path $Path $FileName
        $LoremIpsumContent | Out-File $FilePath -Encoding UTF8 -Force

        # Commande pour ajouter tous les changements (y compris le nouveau fichier)
        Write-Host "Ajout des fichiers au staging..."
        git add .

        # Commande pour effectuer le commit avec la date spécifiée
        $CommitMessage = "$CommitMessagePrefix" + $FormattedDateForMessage
        Write-Host "Effectuer le commit avec le message : '$CommitMessage'..."
        $env:GIT_AUTHOR_DATE = $GitDate
        $env:GIT_COMMITTER_DATE = $GitDate
        git commit -m $CommitMessage

        # Supprimer les variables d'environnement temporaires
        Remove-Item Env:GIT_AUTHOR_DATE
        Remove-Item Env:GIT_COMMITTER_DATE

        # Commande pour pusher vers GitHub (la branche par défaut est 'main' ou 'master')
        Write-Host "Pushing vers GitHub..."
        git push origin HEAD # Changez 'HEAD' par le nom de votre branche si vous n'êtes pas sur la branche par défaut

        Write-Host "Commit et push réussis pour le $FormattedDateForMessage." -ForegroundColor Green

        # Incrémenter la date pour le prochain commit
        $CurrentDate = $CurrentDate.AddDays(1)

        Start-Sleep -Seconds 1 # Petite pause pour éviter de surcharger

    }
}
catch {
    Write-Error "Une erreur est survenue : $($_.Exception.Message)"
}
finally {
    Write-Host "Script terminé." -ForegroundColor Yellow
}