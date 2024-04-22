using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using System.IO;

public enum TransitionDirection {Left, Straight, Right}
public enum PathDirection {Left, Straight, Right}
public enum GenerationMode {InstantAmount, TileForTile}
public class PathGenerator : MonoBehaviour
{
    [SerializeField]Transform playerTransform;
    [SerializeField]Transform pathSectionsFolder;
    [SerializeField]float sectionSizeFactor = 1;
    [Header("For Test Purposes")]
    [SerializeField]GenerationMode generationMode;
    [ShowIf("generationMode", GenerationMode.InstantAmount)][SerializeField]int pathLeght = 10;
    [ShowIf("generationMode", GenerationMode.TileForTile)][SerializeField]float interval = 2;
    [ShowIf("generationMode", GenerationMode.TileForTile)][SerializeField]int startDestroyPathLength = 0;
    [Header("")]

    [SerializeField]List<PathSection> pathSectionPrefabs;
    [SerializeField]PathSection startSection;
    public List<PathSection> availablePathSections = new();
    List<PathSection> leftSections;
    List<PathSection> rightSections;
    PathSection lastSection;
    Queue<PathSection> activePaths = new();
    float hexLength = 0.8660254f;
    [ShowNonSerializedField] PathDirection pathDirection = PathDirection.Straight;

    void Start()
    {
        availablePathSections.AddRange(pathSectionPrefabs);
        leftSections = pathSectionPrefabs.Where(obj => obj.GetDirection() == TransitionDirection.Left).ToList();
        rightSections = pathSectionPrefabs.Where(obj => obj.GetDirection() == TransitionDirection.Right).ToList();

        if(playerTransform != null && pathSectionsFolder != null)
        {
            pathSectionsFolder.position = playerTransform.position - Vector3.up*sectionSizeFactor/2;
            pathSectionsFolder.rotation = playerTransform.rotation;
        }

        if(generationMode == GenerationMode.InstantAmount)
            GenerateWholePath();
        else if(generationMode == GenerationMode.TileForTile)
            StartCoroutine(GenerateTileForTile(2));
    }

//=============== GENERATION MODES FOR TESTING ===============
    void GenerateWholePath()
    {
        if(activePaths.Count <= 0)

        for(int i = 0;i < pathLeght; i++)
            GenerateSection();
    }
    IEnumerator GenerateTileForTile(float interval)
    {
        while(true)
        {
            yield return new WaitForSeconds(this.interval);
            GenerateSection();
        }
    }

//=============== PATH GENERATION ===============
    void GenerateSection() //Creates a new path section to be placed 
    {
        if (lastSection == null)
        {
            lastSection = Instantiate(startSection);
            lastSection.transform.localScale = new Vector3(
                lastSection.transform.localScale.x * sectionSizeFactor,
                lastSection.transform.localScale.y * sectionSizeFactor,
                lastSection.transform.localScale.z * sectionSizeFactor
            );

            activePaths.Enqueue(lastSection);
            lastSection.transform.eulerAngles = new Vector3(0, 90, 0);
            pathDirection = PathDirection.Straight;
            AddToFolder(lastSection);
            lastSection.transform.localPosition = Vector3.zero;
            return;
        }

        int angle = 0;
        switch (lastSection.GetDirection())
        {
            case TransitionDirection.Left:
                angle = -60;
                if (pathDirection == PathDirection.Right)
                    pathDirection = PathDirection.Straight;
                else
                    pathDirection = PathDirection.Left;
                break;
            case TransitionDirection.Right:
                angle = 60;
                if (pathDirection == PathDirection.Left)
                    pathDirection = PathDirection.Straight;
                else
                    pathDirection = PathDirection.Right;
                break;
            case TransitionDirection.Straight:
                break;
        }

        CalculateAvailablePaths();

        PathSection currentSection = Instantiate(availablePathSections[Random.Range(0, availablePathSections.Count)]);
        currentSection.transform.localScale = new Vector3(
                currentSection.transform.localScale.x * sectionSizeFactor,
                currentSection.transform.localScale.y * sectionSizeFactor,
                currentSection.transform.localScale.z * sectionSizeFactor
            );
        AddToFolder(currentSection);

        TransformSection(angle, lastSection, currentSection);

        if (!activePaths.Contains(lastSection))
            activePaths.Enqueue(lastSection);

        if (activePaths.Count > startDestroyPathLength && startDestroyPathLength > 0 && generationMode == GenerationMode.TileForTile)
        {
            Destroy(activePaths.Peek().gameObject);
            activePaths.Dequeue();
        }
    }

    void TransformSection(int angle, PathSection lastSection, PathSection curSection) // Transforms upcoming section based on the current one
    {
        Vector3 offset = Vector3.zero;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        curSection.transform.rotation = lastSection.transform.rotation * rotation;

        switch (pathDirection)
        {
            case PathDirection.Left:
                offset = new Vector3(-1.5f*sectionSizeFactor,0,hexLength*sectionSizeFactor);
                break;
            case PathDirection.Straight:
                offset = new Vector3(0,0,hexLength*2*sectionSizeFactor);
                break;
            case PathDirection.Right:
                offset = new Vector3(1.5f*sectionSizeFactor,0,hexLength*sectionSizeFactor);
                break;
        }
        curSection.transform.position = lastSection.transform.position + offset;
        this.lastSection = curSection;
    }

    void CalculateAvailablePaths() //Defines possible section types for the upcoming section
    {
        Debug.Log("current Directions: "+ lastSection.GetDirection());
        
        if(pathDirection == PathDirection.Straight)
        {
            availablePathSections.Clear();
            availablePathSections.AddRange(pathSectionPrefabs);
            Debug.Log("Path Straight");
        }

        if (lastSection.GetDirection() == TransitionDirection.Left)
        {
            availablePathSections.RemoveAll(section => section.GetDirection() == TransitionDirection.Left);
            if(!availablePathSections.Intersect(rightSections).Any())
                availablePathSections.AddRange(rightSections);
    
            Debug.Log("kicked left");
        }
        else if (lastSection.GetDirection() == TransitionDirection.Right)
        {
            availablePathSections.RemoveAll(section => section.GetDirection() == TransitionDirection.Right);
            if(!availablePathSections.Intersect(leftSections).Any())
                availablePathSections.AddRange(leftSections);
            
            Debug.Log("kicked right");
        }
    }
//=============== STRUCTURE ===============
    void AddToFolder(PathSection ps) => ps.transform.SetParent(GameObject.Find("Path Sections Folder").transform);
}