using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

public enum TransitionDirection {Left, Straight, Right}
public enum PathDirection {Left, Straight, Right}
public enum GenerationMode {InstantAmount, TileForTile}
public class PathGenerator : MonoBehaviour
{
    [Header("For Test Purposes")]
    [SerializeField]int startDestroyPathLength = 0;
    [SerializeField]GenerationMode generationMode;
    [ShowIf("generationMode", GenerationMode.InstantAmount)][SerializeField]int pathLeght = 10;
    [ShowIf("generationMode", GenerationMode.TileForTile)][SerializeField]float interval = 2;
    [Header("")]

    [SerializeField]PathSection[] pathSectionPrefabs;
    [SerializeField]PathSection startSection;
    List<PathSection> availablePathSections = new();
    List<PathSection> leftSections;
    List<PathSection> rightSections;
    PathSection currentSection;
    Queue<PathSection> activePaths = new();
    float hexLength = 0.8660254f;
    [ShowNonSerializedField] PathDirection pathDirection = PathDirection.Straight;

    void Start()
    {
        availablePathSections.AddRange(pathSectionPrefabs);
        leftSections = pathSectionPrefabs.Where(obj => obj.GetDirection() == TransitionDirection.Left).ToList();
        rightSections = pathSectionPrefabs.Where(obj => obj.GetDirection() == TransitionDirection.Right).ToList();

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
        if(currentSection == null)
        {
            currentSection = Instantiate(startSection);
            activePaths.Enqueue(currentSection);
            currentSection.transform.eulerAngles = new Vector3(0,90,0);
            pathDirection = PathDirection.Straight;
            AddToFolder(currentSection);
            return;
        }
        
        PathSection nextSection = Instantiate(availablePathSections[Random.Range(0,availablePathSections.Count)]);    
        AddToFolder(nextSection);
        
        int angle = 0;
        switch (currentSection.GetDirection())
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

        TransformSection(angle,currentSection,nextSection);
        CalculateAvailablePaths();

        if(!activePaths.Contains(currentSection))
            activePaths.Enqueue(currentSection);
        
        if(activePaths.Count > startDestroyPathLength && startDestroyPathLength > 0 )
        {
            Destroy(activePaths.Peek().gameObject);
            activePaths.Dequeue();
        }
    }

    void TransformSection(int angle, PathSection curSection, PathSection nextSection) // Transforms upcoming section based on the current one
    {
        Vector3 offset = Vector3.zero;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        nextSection.transform.rotation = curSection.transform.rotation * rotation;

        switch (pathDirection)
        {
            case PathDirection.Left:
                offset = new Vector3(-1.5f,0,hexLength);
                break;
            case PathDirection.Straight:
                offset = new Vector3(0,0,hexLength*2);
                break;
            case PathDirection.Right:
                offset = new Vector3(1.5f,0,hexLength);
                break;
        }
        nextSection.transform.position = curSection.transform.position + offset;
        currentSection = nextSection;
    }

    void CalculateAvailablePaths() //Defines possible section types for the upcoming section
    {
        Debug.Log("current Directions: "+ currentSection.GetDirection());
        if (currentSection.GetDirection() == TransitionDirection.Left)
        {
            availablePathSections.RemoveAll(section => section.GetDirection() == TransitionDirection.Left);
            if(!availablePathSections.Intersect(rightSections).Any())
                availablePathSections.AddRange(rightSections);

            Debug.Log("kicked left");
        }
        else if (currentSection.GetDirection() == TransitionDirection.Right)
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